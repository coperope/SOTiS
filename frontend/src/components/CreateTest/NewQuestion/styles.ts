import { Theme, createStyles, makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    heading: {
      fontSize: theme.typography.pxToRem(20),
      fontWeight: theme.typography.fontWeightRegular,
    },
    root: {
      flexGrow: 1,
      justify:"center",
      alignItems:"center"
    },
    titlePanel: {
      backgroundColor: "#64B5F6",
      borderRadius: "2px"
    },
    title: {
      flexGrow: 1,
    },
    question: {
      flexGrow: 1,
      marginTop: "2em",
      marginLeft: "4em", 
      marginRight: "4em",
    },
    answerPanel: {
      backgroundColor: "#CAD6DF",
      padding: "0.35em",
    },
    answerText: {
      textAlign: "left",
      fontSize: theme.typography.pxToRem(17),
      fontWeight: theme.typography.fontWeightRegular,
      cursor: "pointer"
    },
  }),
);

export { useStyles };