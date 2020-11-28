import React from 'react';
import { Accordion, AccordionSummary, AccordionDetails, Typography, } from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import { useStyles } from './styles'

interface TestAccordionProps {
  testId: number,
  title: string,
  description: string,
  status: string,
  role: string
}

const TestAccordion = (props: TestAccordionProps) => {
  const classes = useStyles();

  return (
    <Accordion>
      <AccordionSummary
        expandIcon={<ExpandMoreIcon />}
        aria-controls="panel1a-content"
        id="panel1a-header"
      >
        <Typography className={classes.heading}>{props.title}</Typography>
      </AccordionSummary>
      <AccordionDetails>
        <Typography>
          {props.description}
        </Typography>
      </AccordionDetails>
    </Accordion>
  );
}
export default TestAccordion;