import { Theme, createStyles, makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    heading: {
      fontSize: theme.typography.pxToRem(20),
      fontWeight: theme.typography.fontWeightRegular,
    },
    root: {
      flexGrow: 1,
      marginTop: "2em",
      marginBottom: "2em",
    },
    title: {
      flexGrow: 1,
    },
    description: {
      flexGrow: 1,
      marginTop: "2em",
      marginLeft: "4em", 
      marginRight: "4em",
    },
    summary: {
      backgroundColor: "#CAD6DF",
      "&:hover":{
        backgroundColor: "#64B5F6",
      }
    },
    button: {
      backgroundColor: "#CAD6DF",
      color: "#000",
      "&:hover":{
        backgroundColor: "#64B5F6",
      }
    }
  }),
);

export { useStyles };