import React, { useState, useEffect } from 'react';
import { useParams, useHistory } from 'react-router-dom';
import {
  IEdge,
  INode,
} from 'react-digraph';
import { Accordion, AccordionSummary, AccordionDetails, AccordionActions, Typography, Button, Grid, InputLabel, FormControl, Input } from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import { Graph, IGraphProps, IGraph } from '../Graph/Graph'
import { useStyles } from './styles'
import {
  BASE_URL,
  CREATE_KNOWLEDGE_SPACE,
  GET_ONE_KNOWLEDGE_SPACE,
  CREATE_REAL,
} from '../../utils/apiUrls';
import { getToken } from '../../utils/authUtils';
import { getUser } from '../../utils/authUtils';

const submit = async (knowledgeSpace: KnowledgeSpace) => {
  try {
    const token = getToken();
    const options = {
      headers: {
        Authorization: `Bearer ${token}`,
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: "post",
      body: JSON.stringify(knowledgeSpace),
    }
    const response = await fetch(BASE_URL + CREATE_KNOWLEDGE_SPACE(getUser().id), options);
    const result = await response.json();
    return result;
  } catch (error) {
    return false;
  }
}

const get = async (id: string) => {
  try {
    const token = getToken();
    const options = {
      headers: {
        Authorization: `Bearer ${token}`,
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: "get",
    }
    const response = await fetch(BASE_URL + GET_ONE_KNOWLEDGE_SPACE(getUser().id, id), options);
    const result = await response.json();
    return result;
  } catch (error) {
    return false;
  }
}

const createReal = async (id: string) => {
  try {
    const token = getToken();
    const options = {
      headers: {
        Authorization: `Bearer ${token}`,
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: "get",
    }
    const response = await fetch(BASE_URL + CREATE_REAL(getUser().id, id), options);
    const result = await response.json();
    return result;
  } catch (error) {
    return false;
  }
}

interface ParamTypes {
  id: string
}

const initialNode: INode = {
  id: Math.floor(Math.random() * 1000),
  title: '',
  x: 258,
  y: 258
}

interface KnowledgeSpace {
  title: string,
  problems: Array<any>,
  edges: Array<any>
}

const initialKnowledgeSpace: KnowledgeSpace = {
  title: "",
  problems: [],
  edges: []
}

export const CreateKnowledgeSpace = () => {
  const classes = useStyles();
  const history = useHistory();
  const { id } = useParams<ParamTypes>();
  const [graph, setGraph] = useState<IGraph>({ nodes: [], edges: [] });
  const [realGraphs, setRealGraphs] = useState<IGraph[]>([]);

  const [newNode, setNewNode] = useState<INode>(initialNode);
  const [title, setTitle] = useState<string>("");

  useEffect(() => {
    async function fetch() {
      const response = await get(id);
      const knowledgeSpace = response.knowledgeSpaces.shift();
      setGraph({
        edges: knowledgeSpace.edges.map((e: any) => {
          return {
            ...e,
            id: e.edgeId,
            source: e.problemSourceId,
            target: e.problemTargetId,
          }
        }),
        nodes: knowledgeSpace.problems.map((p: any) => {
          return {
            ...p,
            id: p.problemId,
          }
        }),
      });
      setTitle(knowledgeSpace.title);

      let realSpaces = [];
      for (let ks of response.knowledgeSpaces) {
        realSpaces.push(
          {
            edges: ks.edges.map((e: any) => {
              return {
                ...e,
                id: e.edgeId,
                source: e.problemSourceId,
                target: e.problemTargetId,
              }
            }),
            nodes: ks.problems.map((p: any) => {
              return {
                ...p,
                id: p.problemId,
              }
            }),
          }
        );
      }
      setRealGraphs(realSpaces);
    }
    if (id) {
      fetch()
    }
  }, [id]);

  const createNode = () => {
    const node = { ...newNode };
    const nodes = [...graph.nodes, node];
    setNewNode({ ...initialNode, id: Math.floor(Math.random() * 1000) });
    setGraph({
      ...graph,
      nodes: nodes,
    }
    );
  }

  const onChangeProblemTitle = (e: any) => {
    setNewNode(prev => {
      return {
        ...prev,
        title: e.target.value
      }
    });
  }

  const onChangeTitle = (e: any) => {
    setTitle(e.target.value);
  }

  const createKnowledgeSpace = async (graph: IGraph) => {
    let toSubmit: KnowledgeSpace = { ...initialKnowledgeSpace, title: title }
    toSubmit.edges = graph.edges.map(e => {
      return {
        edgeId: Math.floor(Math.random() * 1000),
        problemSourceId: e.source,
        problemTargetId: e.target
      }
    });
    toSubmit.problems = graph.nodes.map(n => {
      return {
        ...n,
        problemId: n.id,
      }
    });

    console.log("knowledgeSpace", toSubmit);
    const result = await submit(toSubmit);
    if (result) {
      history.push(`/knowledge-space/${result.id}`)
    }
  }

  const createRealKs = async () => {
    const result = await createReal(id);
    window.location.reload();
  }

  return (
    <div className={classes.root}>
      <Grid container spacing={1} justify={"center"} alignItems={"center"} style={{ paddingBottom: "2em" }}>
        <Grid item xs={6} style={{ textAlign: "center", paddingBottom: "2em" }}>
          <Typography variant="h4" className={classes.title}>
            Knowledge Space
          </Typography>
          <FormControl fullWidth>
            <InputLabel htmlFor="Text"> Title </InputLabel>
            <Input name="Text" value={title} onChange={(e) => onChangeTitle(e)} inputProps={{ 'aria-label': 'description' }} fullWidth={true} required={true} disabled={id != undefined} />
          </FormControl>
          <Grid item xs={12} style={{ textAlign: "center", paddingTop: "2em" }}>
            {!id &&
              <Accordion>
                <AccordionSummary
                  expandIcon={<ExpandMoreIcon />}
                  aria-controls="panel1a-content"
                  id="panel1a-header"
                >
                  <Typography>Add new problem</Typography>
                </AccordionSummary>
                <AccordionDetails>
                  <Grid container spacing={1}>
                    <Grid item xs={12}>
                      <FormControl fullWidth>
                        <InputLabel htmlFor="Text"> Title </InputLabel>
                        <Input name="Text" value={newNode.title} onChange={(e) => onChangeProblemTitle(e)} inputProps={{ 'aria-label': 'description' }} fullWidth={true} required={true} />
                      </FormControl>
                    </Grid>
                  </Grid>
                </AccordionDetails>
                <AccordionActions>
                  <Button onClick={() => createNode()} variant="contained" color="primary" className={classes.button}>
                    Add
                  </Button>
                </AccordionActions>
              </Accordion>
            }
            {(id && realGraphs?.length === 0) &&
              <Button onClick={() => createRealKs()} variant="contained" color="primary" className={classes.button}>
                Create real
              </Button>
            }
            {(id && realGraphs?.length !== 0 && realGraphs[0].nodes.length === 0) &&
              <Typography>Calculating real knowledge state</Typography>
            }
          </Grid>

        </Grid>

        <Grid item xs={10} style={{ textAlign: "center" }}>
          <Graph graph={graph} createKnowledgeSpace={createKnowledgeSpace} id={id}></Graph>
        </Grid>

        {realGraphs?.map((graph: IGraph, index: number) => (
          <Grid item xs={10} key={index} style={{ textAlign: "center" }}>
            <Typography>Real Knowledge Space</Typography>
            <Graph graph={graph} createKnowledgeSpace={() => { }} id={id}></Graph>
          </Grid>
        ))}
        {!id &&
          <Grid item xs={7} style={{ textAlign: "center", marginTop: "2.5em" }}>
            <Typography variant="subtitle1" >
              To add a problem, click on "Add new problem", enter title and press "ADD".
          </Typography>
            <Typography variant="subtitle1" >
              To add relation, hold shift and click/drag to between problems.
          </Typography>
            <Typography variant="subtitle1" >
              To delete a relation or a problem, click on it and press delete.
          </Typography>
            <Typography variant="subtitle1" >
              Click and drag problems to change their position.
          </Typography>
          </Grid>
        }
      </Grid>
    </div>
  );
};
