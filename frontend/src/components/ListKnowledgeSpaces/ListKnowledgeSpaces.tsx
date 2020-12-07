import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom'
import {
  Accordion, AccordionSummary, AccordionDetails, AccordionActions, Typography, Button, Grid
} from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import {
  BASE_URL,
  GET_ALL_TESTS,
} from '../../utils/apiUrls';
import useFetch from '../../hooks/useFetch';
import { useStyles } from './styles'


const ListKnowledgeSpaces = () => {
  const classes = useStyles();
  const [tests, setTests] = useState([]);
  const { data } = useFetch(BASE_URL + GET_ALL_TESTS, "get");
  const history = useHistory();

  useEffect(() => {
    if (data) {
      setTests(data.tests);
    }
  }, [data]);

  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Typography variant="h4" className={classes.title}>
            My Knowledge Spaces
          </Typography>
          <Button onClick={() => history.push(`/knowledge-space`)} variant="contained" color="primary" className={classes.button} style={{marginTop: "2em"}}>
            Create New
          </Button>
        </Grid>
        <Grid container
          spacing={3}
          justify="center"
          alignItems="center"
          style={{ paddingTop: "3em" }} >

          {tests.map((test: any) => (
            <Grid item xs={8} key={test.testId}>
              <Accordion>
                <AccordionSummary
                  expandIcon={<ExpandMoreIcon />}
                  aria-controls="panel1a-content"
                  id="panel1a-header"
                  className={classes.summary}
                >
                  <Typography className={classes.heading}>{test.title}</Typography>
                </AccordionSummary>
                <AccordionDetails>
                  <Grid container spacing={1}>
                    <Grid item xs={12}>
                      <Typography>
                        {/* {props.description} */}
                      </Typography>
                    </Grid>
                  </Grid>
                </AccordionDetails>
                <AccordionActions>
                  <Button onClick={() => history.push(`/knowledge-space/${test.testId}`)} variant="contained" color="primary" className={classes.button}>
                    Show
                </Button>
                </AccordionActions>
              </Accordion>
            </Grid>

          ))}
        </Grid>
      </Grid>
    </div>
  );
}

export default ListKnowledgeSpaces;
