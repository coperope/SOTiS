import React from 'react';
import clsx from 'clsx';
import { makeStyles, useTheme } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import IconButton from '@material-ui/core/IconButton';
import MenuIcon from '@material-ui/icons/Menu';
import AccountCircle from '@material-ui/icons/AccountCircle';
/* import Switch from '@material-ui/core/Switch';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormGroup from '@material-ui/core/FormGroup'; */
import MenuItem from '@material-ui/core/MenuItem';
import Menu from '@material-ui/core/Menu';
import './Header.css'
import Drawer from '@material-ui/core/Drawer';
import List from '@material-ui/core/List';
import Divider from '@material-ui/core/Divider';
import ChevronLeftIcon from '@material-ui/icons/ChevronLeft';
import ChevronRightIcon from '@material-ui/icons/ChevronRight';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import LocalLibraryIcon from '@material-ui/icons/LocalLibrary';
import AssignmentIcon from '@material-ui/icons/Assignment';
import SchoolIcon from '@material-ui/icons/School';
import PlaylistAdd from '@material-ui/icons/PlaylistAdd';
import BubbleChart from '@material-ui/icons/BubbleChart';
import { Link } from 'react-router-dom'
import ClickAwayListener from '@material-ui/core/ClickAwayListener';
import { getUserPermission, getUser, logout } from '../../utils/authUtils';
import { useHistory } from 'react-router';
const drawerWidth = 240;

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
  },
  barColor: {
    background: '#64b5f6',
  },
  drawer: {
    width: drawerWidth,
    flexShrink: 0,
  },
  drawerPaper: {
    width: drawerWidth,
  },
  drawerHeader: {
    display: 'flex',
    alignItems: 'center',
    padding: theme.spacing(0, 1),
    // necessary for content to be below app bar
    ...theme.mixins.toolbar,
    justifyContent: 'flex-end',
  },
  hide: {
    display: 'none',
  }
}));

function Header() {
  const history = useHistory();
  const classes = useStyles();
  const theme = useTheme();
  const [auth, setAuth] = React.useState(true);
  const [anchorElProfile, setanchorElProfile] = React.useState(null);
  const openProfileMenu = Boolean(anchorElProfile);
  const [open, setOpen] = React.useState(false);

  const handleDrawerOpen = () => {
    if (getUser()) {
      setOpen(true);
    }
  };

  const handleDrawerClose = () => {
    setOpen(false);
  };
  const handleMenuProfile = (event) => {
    setanchorElProfile(event.currentTarget);
  };

  const handleClose = () => {
    setanchorElProfile(null);
  };
  const logOut = () => {
    handleClose();
    logout();
    redirect('login');
  };
  const redirect = (path) => {
    handleClose();
    history.push("/" + path);
  }

  return (
    <div className={classes.root}>
      <AppBar position="static" classes={{ colorPrimary: classes.barColor }}>
        <Toolbar>
          <ClickAwayListener onClickAway={handleDrawerClose}>
            <IconButton
              edge="start"
              color="inherit"
              aria-label="menu"
              onClick={handleDrawerOpen}
              className={clsx(classes.menuButton)}
            >
              <MenuIcon />
            </IconButton>
          </ClickAwayListener>
          <Typography variant="h6" className={classes.title}>
            Examinator
          </Typography>
          {auth && (
            <div>
              <IconButton
                aria-label="account of current user"
                aria-controls="menu-appbar"
                aria-haspopup="true"
                onClick={handleMenuProfile}
                edge="start"
                color="inherit"
                className={clsx(classes.menuButton)}
              >
                <AccountCircle />
              </IconButton>
              <Menu
                id="menu-links"
                anchorEl={anchorElProfile}
                anchorOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                keepMounted
                transformOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                open={openProfileMenu}
                onClose={handleClose}
              >
                {!getUser() &&
                  <div>
                    <MenuItem onClick={() => redirect('login')}>Log in</MenuItem>
                    <MenuItem onClick={() => redirect('register')}>Register</MenuItem>
                  </div>
                }
                {getUser() &&
                  <div>
                    <MenuItem onClick={() => redirect('account')}>My account</MenuItem>
                    <MenuItem onClick={logOut}>Log out</MenuItem>
                  </div>
                }
              </Menu>
            </div>
          )}
        </Toolbar>
      </AppBar>

      {getUser() &&

        <Drawer
          className={classes.drawer}
          variant="persistent"
          anchor="left"
          open={open}
          classes={{
            paper: classes.drawerPaper,
          }}
        >
          <div className={classes.drawerHeader}>
            <Typography variant="h6" className={classes.title}>
              Examinator
          </Typography>
            <IconButton onClick={handleDrawerClose}>
              {theme.direction === 'ltr' ? <ChevronLeftIcon /> : <ChevronRightIcon />}
            </IconButton>
          </div>
          <Divider />
          {getUserPermission() === 0 &&
            <List>
              {['My tests', 'All tests'].map((text, index) => (
                <ListItem button key={text} component={Link} to={index % 2 === 0 ? '/myTests' : '/student/tests'}>
                  <ListItemIcon>{index % 2 === 0 ? <AssignmentIcon /> : <LocalLibraryIcon />}</ListItemIcon>
                  <ListItemText primary={text} />
                </ListItem>
              ))}
            </List>
          }
          <Divider />
          {getUserPermission() === 1 &&
          <>
            <List>
              {['Create test', 'All tests'].map((text, index) => (
                <ListItem button key={text} component={Link} to={index % 2 === 0 ? '/createTest' : '/student/tests'}>
                  <ListItemIcon>{index % 2 === 0 ? <AssignmentIcon /> : <LocalLibraryIcon />}</ListItemIcon>
                  <ListItemText primary={text} />
                </ListItem>
              ))}
            </List>
            <Divider />
            <List>
            {['Create Knowledge Space', 'My Knowledge Spaces'].map((text, index) => (
              <ListItem button key={text} component={Link} to={index % 2 === 0 ? '/knowledge-space' : '/view-knowledge-spaces'}>
                <ListItemIcon>{index % 2 === 0 ? <PlaylistAdd /> : <BubbleChart />}</ListItemIcon>
                <ListItemText primary={text} />
              </ListItem>
            ))}
          </List>
          </>
          }
          <Divider />
          <List>
            {['Status'].map((text, index) => (
              <ListItem button key={text} >
                <ListItemIcon>{<SchoolIcon />}</ListItemIcon>
                <ListItemText primary={text} />
              </ListItem>
            ))}
          </List>
        </Drawer>

      }

    </div>
  );
}

export default Header;
