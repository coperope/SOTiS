import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Grid, Typography } from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import {
  BASE_URL,
  GET_SINGLE_TEST_STUDENT,
} from '../../utils/apiUrls';
import { getUser } from '../../utils/authUtils';
import useFetch from '../../hooks/useFetch';

import { useStyles } from './styles'

interface ParamTypes {
  testId: string
}

interface TestData {
  testId: string,
  title: string,
  description: string,
  professor: any,
  questions: Array<Questions>
}

interface Questions {
  questionId: string,
  text: string,
  answers: Array<Answers>,
  selectedAnswers: Array<Answers>
}

interface Answers {
  answerId: string,
  text: string,
  correct: boolean,
}

const TestView = () => {
  const classes = useStyles();
  const { testId } = useParams<ParamTypes>();
  const { data, executeFetch } = useFetch(BASE_URL + GET_SINGLE_TEST_STUDENT(getUser().id, testId), "get");
  const [ test, setTest ] = useState<TestData>(data?.test);

  useEffect(() => {
    executeFetch(BASE_URL + GET_SINGLE_TEST_STUDENT(getUser().id, testId), "get");
    setTest(data?.test);
  }, []);
  console.log(test);

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

            

        </Grid>
      </Grid>
    </div>
  );
};

export default TestView;