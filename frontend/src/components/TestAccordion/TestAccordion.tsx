import React from 'react';
import { useHistory } from 'react-router-dom'
import { Accordion, AccordionSummary, AccordionDetails, AccordionActions, Typography, Button, Grid, Link } from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import { useStyles } from './styles'

interface TestAccordionProps {
  testId: number,
  title: string,
  description: string,
  completed: string,
  professor: string,
}

const TestAccordion = (props: TestAccordionProps) => {
  const classes = useStyles();
  const history = useHistory();

  return (
    <Accordion>
      <AccordionSummary
        expandIcon={<ExpandMoreIcon />}
        aria-controls="panel1a-content"
        id="panel1a-header"
        className={classes.summary}
      >
        <Typography className={classes.heading}>{props.title}</Typography>
      </AccordionSummary>
      <AccordionDetails>
        <Grid container spacing={1}>
          <Grid item xs={12}>
            <Typography>
              Professor: {props.professor}
            </Typography>
          </Grid>
          <Grid item xs={12}>
            <Typography>
              {props.description}
            </Typography>
          </Grid>
        </Grid>
      </AccordionDetails>
      <AccordionActions>
        {props.completed ?
          <Button onClick= {() => history.push(`/student/test/${props.testId}`) } variant="contained" color="primary" className={classes.button}>
            Show results
          </Button>
          :
          <Button onClick= {() => history.push(`/student/test/${props.testId}`) } variant="contained" color="primary" className={classes.button}>
            Enroll
          </Button>
        }

      </AccordionActions>
    </Accordion>
  );
}
export default TestAccordion;