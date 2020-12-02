import React, { useEffect, useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Input from '@material-ui/core/Input';
import InputLabel from '@material-ui/core/InputLabel';
import NewQuestion from './NewQuestion/NewQuestion';
import {
  Typography,
  Grid,
  FormControl,
  Divider,
  Button
} from '@material-ui/core';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    marginTop: "2em",
    marginBottom: "2em",
  },
  title: {
    flexGrow: 1,
  },
  titleTest: {
    textAlign: "center"
  }
}));
export interface Question {
  Text: string,
  isMultipleChoice: boolean,
  Answers: Array<Answer>
}
export interface Answer {
  Text: string,
  Correct: boolean,
}
export interface Test {
  Title: string,
  Description: string,
  Questions: Array<Question>,
  ProfessorId: string
}
function CreateTest() {
  const classes = useStyles();
  const [questions, setQuestions] = useState<Array<Question>>(Array<Question>());
  const blankQuestion = {
    Text: '',
    isMultipleChoice: false,
    Answers: Array<Answer>()
  }
  const [test, setTest] = useState<Test>({
    Title: '',
    Description: '',
    Questions: Array<Question>(),
    ProfessorId: ''
  });
  const onChange = (e: any) => {
    setTest({
      ...test,
      [e.target.name]: e.target.value
    });
    showTest()
  }

  const addQuestion = (e: any) => {
    setQuestions([...questions, { ...blankQuestion }]);
    showTest();
  };
  const setQuestionText = (text: string, index: number) => {
    setQuestions([
      ...questions.slice(0, index),
      {
        ...questions[index],
        Text: text,
      },
      ...questions.slice(index + 1)
    ]);
  }
  const setQuestionIsMultiple = (isMultiple: boolean, index: number) => {
    setQuestions([
      ...questions.slice(0, index),
      {
        ...questions[index],
        isMultipleChoice: isMultiple,
      },
      ...questions.slice(index + 1)
    ]);
  }
  useEffect(() => {
    setTest({
      ...test,
      Questions: questions
    });
  }, [questions]);

  const showTest = () => {
    console.log(test);
  }

  return <div className={classes.root}>
    <Grid container spacing={3}>
      <Grid item xs={12}>
        <Typography variant="h4" className={classes.title}>
          Create a test
        </Typography>
      </Grid>
      <form className={classes.title}>
        <Grid container
          alignItems='center'
          justify='center'
          spacing={3}
          style={{ paddingTop: "3em", marginBottom: "1em" }} >

          <Grid item xs={7}>
            <FormControl fullWidth>
              <InputLabel className={classes.titleTest} htmlFor="testTitle"> Title</InputLabel>
              <Input className={classes.titleTest} name="Title" value={test.Title} onChange={(e) => onChange(e)} inputProps={{ 'aria-label': 'description', style: { textAlign: 'center' } }} fullWidth={true} required={true} />
            </FormControl>
          </Grid>
          <Divider />
          <Grid item xs={7}>
            <Divider />
            <FormControl fullWidth>
              <InputLabel htmlFor="Description"> Description</InputLabel>
              <Input name="Description" rows={4} multiline value={test.Description} onChange={(e) => onChange(e)} inputProps={{ 'aria-label': 'description' }} fullWidth={true} required={true} />
            </FormControl>
          </Grid>
        </Grid>
        <Divider />
        <Grid container
          alignItems='center'
          justify='center'
          spacing={3}
          style={{ paddingTop: "3em" }} >
          <Grid item xs={12}>
            <Typography variant="h5" className={classes.title}>
              Questions
            </Typography>
          </Grid>

          {questions.map((question: Question, index: number) => (
            <Grid item xs={8} key={index} style={{ paddingTop: "2em", paddingBottom: "2em" }}>
              <NewQuestion
                Text={question.Text}
                isMultipleChoice={question.isMultipleChoice}
                Answers={question.Answers}
                index={index}
                setQuestionText={setQuestionText}
                setQuestionIsMultiple={setQuestionIsMultiple}
              />
            </Grid>
          ))}
          <Grid item xs={12}>
            <Button variant="outlined" color="primary" onClick={(e) => addQuestion(e)}>
              Add a question
            </Button>
          </Grid>
        </Grid>

      </form>

    </Grid>
  </div>;
}

export default CreateTest;
