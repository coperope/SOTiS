import React from 'react';
import { useHistory } from 'react-router-dom'
import { Accordion, AccordionSummary, AccordionDetails, AccordionActions, Typography, Button, Grid } from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import { useStyles } from './styles'
import { getUser, getUserPermission } from '../../utils/authUtils';

interface TestAccordionProps {
  testId: number,
  title: string,
  description: string,
  completed: string,
  professor: string,
  ksId: number,
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
        {getUserPermission() === 0 &&
          <>
            <Button onClick={() => history.push(`/student/test/${props.testId}`)} variant="contained" color="primary" className={classes.button}>
              {props.completed ? "Show results" : "Enroll"}
            </Button>
            {!props.completed && <Button onClick={() => history.push(`/student/test/guided/${props.testId}`)} variant="contained" color="primary" className={classes.button}>
              Guided testing
            </Button>
            }
          </>
        }

        {getUserPermission() === 1 &&
          <>
            <Button onClick={() => history.push(`/knowledge-space/${props.ksId}`)} variant="contained" color="primary" className={classes.button}>
              Show knowledge space
            </Button>
            <Button onClick={() => window.open(`http://localhost:5000/professor/${getUser().id}/tests/${props.testId}/qti`, "_blank")} variant="contained" color="primary" className={classes.button}>
              Export QTI
            </Button>
          </>
        }

      </AccordionActions>
    </Accordion>
  );
}
export default TestAccordion;