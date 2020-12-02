import React, {useState, useEffect} from 'react';
import { Question, Answer } from '../CreateTest'
import { useStyles } from './styles'
import { Grid, Typography, Checkbox, FormControl, InputLabel, Input, Button } from '@material-ui/core';

interface QuestionProps extends Question {
  index: number,
  setQuestionText: any,
  setQuestionIsMultiple: any
}

function NewQuestion(question: QuestionProps) {

  const classes = useStyles();
  const [answers, setAnswers] = useState<Array<Answer>>(Array<Answer>());
  const blankAnswer: Answer = {
    Text: '',
    Correct: false
  }
  const onChange = (e: any) => {
    question.setQuestionText(e.target.value, question.index);
  }

  const addAnswer = (e: any) => {
    setAnswers((answers) => {
      return [...answers, { ...blankAnswer }];
    });
  }
  useEffect(() => {
    question?.Answers?.splice(0,question?.Answers?.length);
    question?.Answers?.push(...answers);

  }, [answers]);

  return <div className={classes.root}>
    <Grid container spacing={3}>
      <Grid item xs={12} className={classes.titlePanel}>
        <FormControl fullWidth>
          <InputLabel htmlFor="Text"> Question {question.index + 1} text</InputLabel>
          <Input name="Text" value={question.Text} onChange={(e) => onChange(e)} inputProps={{ 'aria-label': 'description' }} fullWidth={true} required={true} />
        </FormControl>
      </Grid>
      <Grid container
        xs={12}
        direction="column"
        justify="center"
        alignItems="stretch" >
        {answers.map((answer: Answer, index: number) => (
          <Grid item xs={8} key={index} style={{ paddingTop: "2em", paddingBottom: "2em" }}>
            answer
          </Grid>
        ))}
        <Grid item xs={12}>
          <Button variant="outlined" color="primary" onClick={() => setAnswers([...answers, { ...blankAnswer }])}>
            Add an answer
          </Button>
        </Grid>
        {/* {question?.answers.map((answer: Answer) => (
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

      ))} */}

      </Grid>
    </Grid>
  </div >;
}

export default NewQuestion;
