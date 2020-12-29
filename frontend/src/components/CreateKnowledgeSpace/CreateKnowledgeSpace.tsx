import React, { useState, useEffect } from 'react';
import { useParams, useHistory } from 'react-router-dom';
import {
  IEdge,
  INode,
} from 'react-digraph';
import { Accordion, AccordionSummary, AccordionDetails, Box, AccordionActions, Divider, Typography, Button, Grid, InputLabel, FormControl, Input } from '@material-ui/core';
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
import GraphConfig, {
  EMPTY_EDGE_TYPE,
  SPECIAL_EDGE_TYPE,
} from '../Graph/graph-config';

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
    console.log(result);
    return result;
  } catch (error) {
    return false;
  }
}

interface ParamTypes {
  id: string
}

const initialNode: INode = {
  id: Math.floor(Math.random() * 10000),
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
  const [graphCompare, setGraphCompare] = useState<IGraph>({ nodes: [], edges: [] })
  const [newNode, setNewNode] = useState<INode>(initialNode);
  const [title, setTitle] = useState<string>("");
  const [levenshteinDistance, setLevenshteinDistance] = useState<number>(0);

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
      let compareSpace: any[] = [];
      
      for (let ks of response.knowledgeSpaces) {
        let nodesMapped: any = ks.problems.map((p: any) => {
          let matchingNode: any = knowledgeSpace.problems.filter((node: any) => p.x == node.x && p.y == node.y);
          return {
            ...matchingNode[0],
            id: matchingNode[0].problemId,
          }
        });
        let edgesMapped = ks.edges.map((e: any) => {
          let problemSource: any = knowledgeSpace.problems.filter((p: any) => {
            return p.x == e.problemSource.x && p.y == e.problemSource.y;
          });
          let problemTarget: any = knowledgeSpace.problems.filter((p: any) => {
            return p.x == e.problemTarget.x && p.y == e.problemTarget.y;
          });
          return {
            ...e,
            id: e.edgeId,
            source: problemSource[0].problemId,
            target: problemTarget[0].problemId,
            type: "specialEdge"
          }
        });
        compareSpace.push(
          {
            edges: edgesMapped,
            nodes: nodesMapped
          }
        );
        break;
      }
      if (compareSpace.length) {
        let compareGraphEdgesGood: any = [];
        let compareGraphEdgesBadReal: any = [];
        let compareGraphEdgesBadExpected: any = [];
        compareGraphEdgesGood = compareSpace[0].edges.filter((e: any) => {
          if (knowledgeSpace.edges.filter((edge: any) => {
            return edge.problemSourceId == e.source && edge.problemTargetId == e.target;
          }).length == 0) {
            compareGraphEdgesBadReal.push({
              ...e,
              source: e.source,
              target: e.target,
              type: "badRealEdge"
            });
          }
          return knowledgeSpace.edges.filter((edge: any) => {
            return edge.problemSourceId == e.source && edge.problemTargetId == e.target;
          }).length;
        });
        knowledgeSpace.edges.filter((edge: any) => {
          if (compareSpace[0].edges.filter((e: any) => {
            return edge.problemSourceId == e.source && edge.problemTargetId == e.target;
          }).length == 0) {
            compareGraphEdgesBadExpected.push({
              ...edge,
              source: edge.problemSourceId,
              target: edge.problemTargetId,
              type: "badExpectedEdge"
            });
          };
          return
        });
        compareSpace[0].edges = compareGraphEdgesGood.concat(compareGraphEdgesBadReal).concat(compareGraphEdgesBadExpected);
        console.log("Real space mapped: ");
      console.log(compareSpace[0]);
      setGraphCompare(compareSpace[0]);
      setLevenshteinDistance(compareGraphEdgesBadExpected.length + compareGraphEdgesBadReal.length);
      }
      
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
  let boxHeight = id ? "690px" : "755px";
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

        <Grid item xs={10} style={{ textAlign: "center", marginTop: "2em" }}>
          <Typography variant="h4" className={classes.title}>
            Expected knowledge Space
          </Typography>
          <Box justifyContent="center" alignItems='center' border={1} boxShadow={3} style={{ minWidth: "80em", maxWidth: "100em", height: boxHeight, background: "#f0f8ff" }}>
            <Graph graph={graph} createKnowledgeSpace={createKnowledgeSpace} id={id}></Graph>
          </Box>
        </Grid>
        <Divider />
        {realGraphs?.map((graph: IGraph, index: number) => (

          <Grid item xs={10} key={index} style={{ textAlign: "center", marginTop: "2em" }}>
            <Typography variant="h4" className={classes.title}>{index == 0 ? "Real Knowledge Space" : "All posible knowledge states"}</Typography>
            <Box justifyContent="center" alignItems='center' border={1} boxShadow={3} style={{ minWidth: "80em", maxWidth: "100em", background: "#f0f8ff" }}>
              <Graph graph={graph} createKnowledgeSpace={() => { }} id={id}></Graph>
            </Box>
          </Grid>
        ))}
        <Divider />
        {graphCompare?.nodes?.length != 0 &&
          <Grid item xs={10} key={1} style={{ textAlign: "center", marginTop: "2em" }}>
            <Typography variant="h4" className={classes.title}>Compared expected vs real knowledge Space</Typography>
            <Box justifyContent="center" alignItems='center' border={1} boxShadow={3} style={{ minWidth: "80em", maxWidth: "100em", background: "#f0f8ff" }}>
              <Graph graph={graphCompare} createKnowledgeSpace={() => { }} id={id}></Graph>
            </Box>
            
            <Typography variant="overline" align="left" style={{ textAlign: "left" }}>
              <Grid container spacing={0} >
                <Grid item xs={12} style={{ padding: "1em", paddingTop: "0.4em" }}>
                  Graph edit distance between expected and real space: <b>{levenshteinDistance}</b>
                </Grid>
              </Grid>
            </Typography>

            <Typography variant="overline" align="left" style={{ textAlign: "left" }}>
              <Grid container spacing={0} >
                <Grid item xs={12} style={{ padding: "1em", paddingTop: "0.4em" }}>
                  <Box border={1} style={{ maxWidth: "1em", maxHeight: "1em", minWidth: "1em", minHeight: "1em", background: "red" }}>
                  </Box>These relations have been wrongly expected.
                </Grid>
              </Grid>
            </Typography>

            <Typography variant="overline" align="left">
            <Grid container spacing={0} >
                <Grid item xs={12} style={{ padding: "1em", paddingTop: "0.4em" }}>
                  <Box border={1} style={{ maxWidth: "1em", maxHeight: "1em", minWidth: "1em", minHeight: "1em", background: "green" }}>
                  </Box>These relations have been wrongly expected.

                </Grid>
              </Grid>
          </Typography>
            <Typography variant="overline" align="left">
            <Grid container spacing={0} >
                <Grid item xs={12} style={{ padding: "1em", paddingTop: "0.4em" }}>
                  <Box border={1} style={{ maxWidth: "1em", maxHeight: "1em", minWidth: "1em", minHeight: "1em", background: "blue" }}>
                  </Box>These relations have been wrongly expected.

                </Grid>
              </Grid>
          </Typography>
          </Grid>

        }
        <Divider />
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
