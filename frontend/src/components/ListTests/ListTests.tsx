import React, { useEffect, useState } from 'react';
import { makeStyles, useTheme } from '@material-ui/core/styles';
import {
  Typography,
  Grid,
} from '@material-ui/core';

import { GET_ALL_TESTS } from '../../utils/apiUrls';
import useFetch from '../../hooks/useFetch';

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
  const fetchData = useFetch(GET_ALL_TESTS, {});


  useEffect(() => {
    async function fetch() {
      const {response, error} = await fetchData();
      console.log(response);
      console.log(error);
    }
    fetch()
   
  }, []);

  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Typography variant="h4" className={classes.title}>
            All tests
        </Typography>
        </Grid>

        <Grid item xs={12} spacing={2}>
          <Typography variant="h6" className={classes.title}>
            All tests
        </Typography>
          <Typography variant="h6" className={classes.title}>
            All tests
        </Typography>
        </Grid>

      </Grid>


    </div>
  );
}

export default ListTests;
