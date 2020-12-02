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
    button: {
      margin: theme.spacing(1),
    },
    titlePanel: {
      backgroundColor: "#c8d9ed",
      borderRadius: "2px"
    },
    questionPanel: {
      backgroundColor: "#f0f8ff",
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
      backgroundColor: "#e1f1fd",
      marginLeft: "2em"
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