import React, { useEffect, useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Input from '@material-ui/core/Input';
import InputLabel from '@material-ui/core/InputLabel';
import {
  Typography,
  Grid,
  FormControl,
  Divider
} from '@material-ui/core';

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

function CreateTest() {
  const classes = useStyles();
  return <div className={classes.root} justify="center"
    alignItems="center">
    <Grid container spacing={3}>
      <Grid item xs={12}>
        <Typography variant="h4" className={classes.title}>
          Create a test
        </Typography>
      </Grid>
      <form className={classes.title}>
        <Grid container
          spacing={3}
          justify="center"
          alignItems="center"
          style={{ paddingTop: "3em" }} >

          <Grid item xs={7}>
            <FormControl fullWidth='true'>
              <InputLabel for="testTitle"> Title</InputLabel>
              <Input name="testTitle" inputProps={{ 'aria-label': 'description' }} fullWidth='true' />
            </FormControl>
          </Grid>
          <Divider />
          <Grid item xs={7}>
            <Divider />
            <FormControl fullWidth='true'>
              <InputLabel for="testTitle"> Description</InputLabel>
              <Input name="testTitle" inputProps={{ 'aria-label': 'description' }} fullWidth='true' />
            </FormControl>
          </Grid>
        </Grid>

      </form>

    </Grid>
  </div>;
}

export default CreateTest;
