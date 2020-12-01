import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Grid, Typography, Button } from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import QuestionView from './Question/Question'
import {
  BASE_URL,
  GET_SINGLE_TEST_STUDENT,
  SUBMIT_TEST,
} from '../../utils/apiUrls';
import { getToken } from '../../utils/authUtils';

import { getUser } from '../../utils/authUtils';
import useFetch from '../../hooks/useFetch';

import { useStyles } from './styles'

interface ParamTypes {
  testId: string
}

export interface TestData {
  testId: string,
  title: string,
  description: string,
  professor: any,
  questions: Array<Question>,
  completed: boolean
}

export interface Question {
  questionId: string,
  text: string,
  answers: Array<Answer>,
  selectedAnswers: Array<Answer>
}

export interface Answer {
  answerId: string,
  text: string,
  correct: boolean,
}

const submit = async (test: TestData) => {
  try {
    const token = getToken();
    const options = {
      headers: {
        Authorization: `Bearer ${token}`,
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: "post",
      body: JSON.stringify({test: test}),
    }
    const response = await fetch(BASE_URL + GET_SINGLE_TEST_STUDENT(getUser().id, test.testId), options);
    const result = await response.json();
    return response.ok
  } catch (error) {
    return false;
  }
}

const TestView = () => {
  const classes = useStyles();
  const { testId } = useParams<ParamTypes>();
  const { data, executeFetch, hasError } = useFetch(BASE_URL + GET_SINGLE_TEST_STUDENT(getUser().id, testId), "get");

  const [test, setTest] = useState<TestData>(data?.test);

  useEffect(() => {
    setTest(data?.test);
  }, [data]);
  console.log(test);

  const submitTest = async () => {
    const submitedTest = JSON.parse(JSON.stringify(test))
    const success = await submit(submitedTest);
    console.log(success);
    if (success) {
      executeFetch(BASE_URL + GET_SINGLE_TEST_STUDENT(getUser().id, testId), "get");
    }
  }

  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Typography variant="h4" className={classes.title}>
            {test?.title}
          </Typography>
          <Typography variant="h6" className={classes.description}>
            {test?.description}
          </Typography>
        </Grid>
        <Grid container
          spacing={3}
          justify="center"
          alignItems="center"
          style={{ paddingTop: "3em" }} >

          {test?.questions.map((question: Question) => (
            <Grid item xs={8} key={question.questionId} style={{ paddingTop: "2em", paddingBottom: "2em" }}>
              <QuestionView
                questionId={question.questionId}
                text={question.text}
                answers={question.answers}
                selectedAnswers={question.selectedAnswers}
                completed={test.completed}
              />
            </Grid>
          ))}
        </Grid>
        <Grid container
          spacing={3}
          justify="center"
          alignItems="center"
          style={{ paddingTop: "6em", paddingBottom: "6em" }} >
          {!test?.completed && <Button onClick={submitTest} variant="contained" color="primary" className={classes.button}>
            Finish test
          </Button>}
        </Grid>
      </Grid>
    </div>
  );
};

export default TestView