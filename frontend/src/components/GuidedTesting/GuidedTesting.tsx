import React, { useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import { Grid, Typography, Button } from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import QuestionView from '../TestView/Question/Question'
import {
  BASE_URL,
  GET_GUIDED_TEST_STUDENT,
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

const GuidedTesting = () => {
  const classes = useStyles();
  const history = useHistory();

  const { testId } = useParams<ParamTypes>();
  const { data, executeFetch, hasError } = useFetch(BASE_URL + GET_GUIDED_TEST_STUDENT(getUser().id, testId), "post", {});

  const [test, setTest] = useState<TestData>(data?.test);

  useEffect(() => {
    if (data?.testToReturn?.questions?.length === 0) {
      history.push(`/student/test/${data?.testToReturn?.testId}`);
    }
    setTest(data?.testToReturn);
    
  }, [data]);

  const nextQuestion = async () => {
    const submitedTest = JSON.parse(JSON.stringify(test))
    await executeFetch(BASE_URL + GET_GUIDED_TEST_STUDENT(getUser().id, testId), "post", submitedTest.questions[0]);
    //console.log(success);
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

            <Grid item xs={8} key={test?.questions[0]?.questionId} style={{ paddingTop: "2em", paddingBottom: "2em" }}>
              <QuestionView
                questionId={test?.questions[0]?.questionId}
                text={test?.questions[0]?.text}
                answers={test?.questions[0]?.answers}
                selectedAnswers={test?.questions[0]?.selectedAnswers}
                completed={false}
              />
            </Grid>
        </Grid>
        <Grid container
          spacing={3}
          justify="center"
          alignItems="center"
          style={{ paddingTop: "6em", paddingBottom: "6em" }} >
          <Button onClick={nextQuestion} variant="contained" color="primary" className={classes.button}>
            Next question
          </Button>
        </Grid>
      </Grid>
    </div>
  );
};

export default GuidedTesting