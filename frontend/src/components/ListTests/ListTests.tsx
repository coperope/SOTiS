import React, { useEffect, useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import {
  Typography,
  Grid,
} from '@material-ui/core';

import { BASE_URL, GET_ALL_TESTS } from '../../utils/apiUrls';
import useFetch from '../../hooks/useFetch';
import TestAccordion from '../TestAccordion/TestAccordion';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    marginTop: "2em",
    marginBottom: "2em",
  },
  title: {
    flexGrow: 1,
  }
}));



const ListTests = () => {
  const classes = useStyles();
  const [tests, setTests] = useState([]);
  const { data } = useFetch(BASE_URL + GET_ALL_TESTS, "get");

  useEffect(() => {
    if (data) {
      setTests(data.tests);
    }
  }, [data]);

  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Typography variant="h4" className={classes.title}>
            All tests
          </Typography>
        </Grid>
        <Grid container
          spacing={3}
          justify="center"
          alignItems="center"
          style={{ paddingTop: "3em" }} >

          {tests.map((test: any) => (
            <Grid item xs={8} key={test.id}>
              <TestAccordion
                testId={test.id}
                title={test.title}
                description={test.description}
                status={"NOT-ENROLED"}
                professor={test.professor.firstName + " " + test.professor.lastName}
              />
            </Grid>
          ))}
        </Grid>


      </Grid>


    </div>
  );
}

export default ListTests;
