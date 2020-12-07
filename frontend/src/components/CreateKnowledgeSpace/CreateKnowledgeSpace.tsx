import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import {
  IEdge,
  INode,
} from 'react-digraph';
import { Accordion, AccordionSummary, AccordionDetails, AccordionActions, Typography, Button, Grid, InputLabel, FormControl, Input } from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import { Graph, IGraphProps, IGraph } from '../Graph/Graph'
import { useStyles } from './styles'

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
  const { id } = useParams<ParamTypes>();
  const [graph, setGraph] = useState<IGraph>({ nodes: [], edges: [] });
  const [newNode, setNewNode] = useState<INode>(initialNode);
  const [knowledgeSpace, setKnowledgeSpace] = useState<KnowledgeSpace>(initialKnowledgeSpace);

  // TODO: get knowledge space

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
    setKnowledgeSpace(prev => {
      return {
        ...prev,
        title: e.target.value
      }
    });
  }

  const createKnowledgeSpace = (graph: IGraph) => {
    setKnowledgeSpace(prev => {
      return {
        ...prev,
        problems: graph.nodes,
        edges: graph.edges,
      }
    });
    console.log(graph);
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
            <Input name="Text" value={knowledgeSpace.title} onChange={(e) => onChangeTitle(e)} inputProps={{ 'aria-label': 'description' }} fullWidth={true} required={true} />
          </FormControl>
          <Grid item xs={12} style={{ textAlign: "center", paddingTop: "2em" }}>

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
          </Grid>

        </Grid>

        <Grid item xs={10} style={{ textAlign: "center" }}>
          <Graph graph={graph} createKnowledgeSpace={createKnowledgeSpace} ></Graph>
        </Grid>

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
      </Grid>
    </div>
  );
};
