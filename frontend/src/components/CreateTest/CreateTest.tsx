import React, { useEffect, useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import NewQuestion from './NewQuestion/NewQuestion';
import { getUser, getToken } from '../../utils/authUtils';
import { BASE_URL, CREATE_TEST_PREFIX, CREATE_TEST_POSTFIX } from '../../utils/apiUrls';
import { useHistory } from 'react-router';
import { Typography, Grid, FormControl, Divider, Button, Box, Input, InputLabel  } from '@material-ui/core';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    marginTop: "2em",
    marginBottom: "2em",
  },
  container: {
    border: "1"
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
  const history = useHistory();
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
  }

  const addQuestion = (e: any) => {
    setQuestions([...questions, { ...blankQuestion }]);
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

  const removeQuestion = (index: number) => {
    console.log(index)
    setQuestions([
      ...questions.slice(0, index),
      ...questions.slice(index + 1)]);
  }

  const onSubmit = (e: any) => {
    e.preventDefault();
    const token = getToken();
    const requestOptions = {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${token}`,
        Accept: 'application/json, text/plain, */*',
        'Content-Type': 'application/json;charset=UTF-8'
      },
      body: JSON.stringify({
        Title: test.Title,
        ProfessorId: getUser().id,
        Description: test.Description,
        Questions: questions
      })
    }
    const url = process.env.NODE_ENV === 'production' ? CREATE_TEST_PREFIX : BASE_URL + CREATE_TEST_PREFIX + getUser().id + CREATE_TEST_POSTFIX;
    fetch(url, requestOptions)
      .then((response) => {
        if (!response.ok) {
          alert("Invalid username or password")
        }
        else {
          alert("Successfuly created test " + test.Title + ".");
          history.push("/tests");
          return response.json();
        }
      })
      .catch((error) => {
        console.log(error);
      });
  }


  return <div className={classes.root}>
    <Grid container spacing={3} alignItems='center' justify='center'>
      <Grid item xs={12}>
        <Typography variant="h4" className={classes.title}>
          Create a test
        </Typography>
      </Grid>

      <form className={classes.title} onSubmit={(e) => onSubmit(e)}  >
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
          style={{ marginTop: "0.1em" }}
        >
          <Box justifyContent="center" alignItems='center' border={1} boxShadow={2} style={{ minWidth: "60em", maxWidth: "60em", background: "#f0f8ff" }}>
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
                <Grid item xs={11} key={index} style={{ paddingTop: "2em", paddingBottom: "2em" }}>
                  <NewQuestion
                    Text={question.Text}
                    isMultipleChoice={question.isMultipleChoice}
                    Answers={question.Answers}
                    index={index}
                    setQuestionText={setQuestionText}
                    setQuestionIsMultiple={setQuestionIsMultiple}
                    remove={removeQuestion}
                  />
                </Grid>
              ))}

              <Grid item xs={12}>
                <Button variant="outlined" color="primary" onClick={(e) => addQuestion(e)} style={{ marginTop: "0.5em", marginBottom: "1em" }}>
                  Add a question
                </Button>
              </Grid>
            </Grid>
          </Box>
        </Grid>
        <Button type="submit" variant="contained" color="primary" style={{ marginTop: "2.5em", marginBottom: "1em" }}>
          Sumbit
        </Button>
      </form>
    </Grid>
  </div>;
}

export default CreateTest;
