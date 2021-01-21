import React, { useState, useEffect } from 'react';
import { Grid, Typography, Checkbox } from '@material-ui/core';

import { Question, Answer } from '../TestView'
import { useStyles } from './styles'

interface QuestionProps extends Question {
  completed: boolean
}

const QuestionView = (question: QuestionProps) => {
  const classes = useStyles();
  const [checkedArray, setCheckedArray] = useState<Array<string>>([]);

  useEffect(() => {
    const selected = question?.selectedAnswers?.map(a => a.answerId);
    setCheckedArray(selected ? selected : []);
  }, [question]);

  const checked = (answerId: string) => {
    return checkedArray?.some(id => id === answerId);
  }

  const selectAnswer = (event: any, answerId: string) => {
    if (question?.completed){
      return;
    }
    const answer = question?.answers?.find(a => a.answerId === answerId);
    if (!answer) {
      return;
    }
    const alreadySelected = checkedArray?.some(id => id === answerId);
    if (!alreadySelected) {
      question?.selectedAnswers?.push(answer);
      setCheckedArray(checkedArray.concat(answerId));
    } else if (alreadySelected) {
      question?.selectedAnswers?.splice(question?.answers?.findIndex(a => a.answerId === answerId), 1);
      setCheckedArray(checkedArray.filter(id => id !== answerId));
    }
  }

  const getCheckboxStyle = (answerId: string) => {
    if (!question?.completed) {
      return {
        color: "#64B5F6"
      };
    } else {
      if (question?.selectedAnswers?.find(a => a.answerId === answerId)) {
        if (question?.answers?.find(a => a.answerId === answerId)?.correct) {
          return {
            color: "#008000"
          };
        } else {
          return {
            color: "#FF0000"
          };
        }
      } else if (question?.answers?.find(a => a.answerId === answerId)?.correct) {
        return {
          color: "#008000",
          borderWidth: "3px"
        };
      }
    }
  }
  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        <Grid item xs={12} className={classes.titlePanel}>
          <Typography variant="h5" className={classes.title}>
            {question.text}
          </Typography>
        </Grid>
        <Grid container
          xs={12}
          direction="column"
          justify="center"
          alignItems="stretch" >

          {question?.answers?.map((answer: Answer) => (
            <Grid container item
              xs={12}
              key={answer.answerId}
              className={classes.answerPanel}
              direction="row"
              justify="flex-start"
              alignItems="center"
            >
              <Grid item xs={1}>
                <Checkbox
                  disabled={question?.completed}
                  checked={checked(answer.answerId)}
                  onChange={(e) => selectAnswer(e, answer.answerId)}
                  inputProps={{ 'aria-label': 'primary checkbox' }}
                  style={getCheckboxStyle(answer.answerId)}
                />
              </Grid>
              <Grid item xs={11}>
                <Typography variant="h6" className={classes.answerText} onClick={(e) => selectAnswer(e, answer.answerId)}>
                  {answer.text}
                </Typography>
              </Grid>
            </Grid>

          ))}

        </Grid>
      </Grid>
    </div>
  );
};

export default QuestionView