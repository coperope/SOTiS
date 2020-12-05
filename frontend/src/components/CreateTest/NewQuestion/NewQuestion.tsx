import React, { useState, useEffect } from 'react';
import { Question, Answer } from '../CreateTest'
import { useStyles } from './styles'
import DeleteIcon from '@material-ui/icons/Delete';
import { Grid, Checkbox, FormControl, InputLabel, Input, Button, FormControlLabel, Box, IconButton } from '@material-ui/core';
import NewAnswer from './NewAnswer/NewAnswer';


interface QuestionProps extends Question {
  index: number,
  setQuestionText: any,
  setQuestionIsMultiple: any,
  remove: any
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
  const setIsMultipleChoice = (e: any) => {
    question.setQuestionIsMultiple(e.target.checked, question.index);
  }

  const removeQuestion = () => {
    console.log(question.index)
    question.remove(question.index);
  }

  useEffect(() => {
    question?.Answers?.splice(0, question?.Answers?.length);
    question?.Answers?.push(...answers);

  }, [answers]);

  const setAnswerText = (text: string, index: number) => {
    setAnswers([
      ...answers.slice(0, index),
      {
        ...answers[index],
        Text: text,
      },
      ...answers.slice(index + 1)
    ]);
  }
  const setAnswerIsCorrect = (isCorrect: boolean, index: number) => {
    setAnswers([
      ...answers.slice(0, index),
      {
        ...answers[index],
        Correct: isCorrect,
      },
      ...answers.slice(index + 1)
    ]);
  }
  const removeAnswer = (index: number) => {
    setAnswers([
      ...answers.slice(0, index),
      ...answers.slice(index + 1)]);
  }

  return <div className={[classes.root, classes.questionPanel].join(' ')}>
    <Box boxShadow={1}>
      <Box boxShadow={1} className={classes.titlePanel} style={{ minWidth: "38.6em", maxWidth: "60em", margin: "0.2em 0 0.5em" }}>
        <Grid container spacing={3} alignItems='center'
          justify='center' >
          <Grid item xs={8} style={{ padding: "0.5em" }}>
            <FormControl fullWidth>
              <InputLabel htmlFor="Text"> Question {question.index + 1} text</InputLabel>
              <Input name="Text" value={question.Text} onChange={(e) => onChange(e)} inputProps={{ 'aria-label': 'description' }} fullWidth={true} required={true} />
            </FormControl>
          </Grid>
          <Grid item xs={2} style={{ padding: "0.5em" }}>
            <FormControlLabel
              control={
                <Checkbox
                  checked={question.isMultipleChoice}
                  onChange={(e) => setIsMultipleChoice(e)}
                  name="Correct"
                  color="primary"
                />
              }
              label="Multiple choice"
            />
          </Grid>
          <Grid item xs={1}>
            <IconButton className={classes.button} onClick={() => removeQuestion()}>
              <DeleteIcon />
            </IconButton >
          </Grid>
        </Grid>
      </Box>
      <Grid container
        direction="column"
        justify="center"
        alignItems="stretch"
        style={{ paddingTop: "1.5em" }} >
        <Grid item xs={12}>
          {answers.map((answer: Answer, index: number) => (
            <Grid item xs={11} key={index} style={{ paddingTop: "1em", paddingBottom: "1em", marginLeft: "0.5em", marginRight: "0.5em" }}>
              <NewAnswer
                Text={answer.Text}
                Correct={answer.Correct}
                index={index}
                setAnswerText={setAnswerText}
                setAnswerIsCorrect={setAnswerIsCorrect}
                remove={removeAnswer}
              ></NewAnswer>
            </Grid>
          ))}
          <Grid item xs={12}>
            <Button variant="outlined" color="primary" onClick={() => setAnswers([...answers, { ...blankAnswer }])} style={{ marginTop: "0.8em", marginBottom: "0.4em" }}>
              Add an answer
            </Button>
          </Grid>
        </Grid>
      </Grid>
    </Box>
  </div >;
}

export default NewQuestion;
