import { Theme, createStyles, makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    marginTop: "2em",
    marginBottom: "2em",
  },
  title: {
    flexGrow: 1,
    paddingBottom: "0.5em",
  },
  button: {
    backgroundColor: "#CAD6DF",
    color: "#000",
    "&:hover":{
      backgroundColor: "#64B5F6",
    }
  }
}));

export { useStyles };