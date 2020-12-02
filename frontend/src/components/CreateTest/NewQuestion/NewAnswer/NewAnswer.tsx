import React, { useState, useEffect } from 'react';
import { Question, Answer } from '../../CreateTest'
import { useStyles } from '../styles'
import DeleteIcon from '@material-ui/icons/Delete';
import { Grid, Typography, Checkbox, FormControl, InputLabel, Input, IconButton, FormControlLabel, Box } from '@material-ui/core';

interface AnswerProps extends Answer {
  index: number,
  setAnswerText: any,
  setAnswerIsCorrect: any,
  remove: any
}

function NewAnswer(answer: AnswerProps) {
  const classes = useStyles();

  const onChange = (e: any) => {
    answer.setAnswerText(e.target.value, answer.index);
  }

  const setCorrect = (e: any) => {
    answer.setAnswerIsCorrect(e.target.checked, answer.index);
  }
  const removeAnswer = () => {
    answer.remove(answer.index);
  }
  return <div className={[classes.answerPanel, classes.root].join(' ')}>
    <Box boxShadow={0}>
    <Grid container spacing={2}>
      <Grid item xs={7} className={[classes.answerPanel, classes.answerText].join(' ')}>
        <FormControl fullWidth>
          <InputLabel htmlFor="Text"> Answer {answer.index + 1} text</InputLabel>
          <Input name="Text" value={answer.Text} onChange={(e) => onChange(e)} inputProps={{ 'aria-label': 'description' }} fullWidth={true} required={true} />
        </FormControl>
      </Grid>
      <Grid item xs={2} >
        <FormControlLabel
          control={
            <Checkbox
              checked={answer.Correct}
              onChange={(e) => setCorrect(e)}
              name="Correct"
              color="primary"
            />
          }
          label="Correct"
        />
      </Grid>
      <Grid item xs={2}>
        <IconButton 
          className={classes.button}
          onClick={() => removeAnswer()}
        >
          <DeleteIcon />
        </IconButton >
      </Grid>
    </Grid>
    </Box>
  </div>;
}

export default NewAnswer;
