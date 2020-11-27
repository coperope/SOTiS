import React from 'react';
import { Link } from 'react-router-dom'
import { makeStyles, useTheme } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
  }
}));

function ListTests() {
  const classes = useStyles();
  return <div >
    <Typography variant="h6" className={classes.title}>
      All tests
    </Typography>


  </div>;
}

export default ListTests;
