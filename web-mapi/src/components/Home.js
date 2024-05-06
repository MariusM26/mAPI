import React from "react";
import { Button, makeStyles, Tooltip } from "@material-ui/core";
import LogoutLink from "../components/LogoutLink.js";
import AuthorizeView, { AuthorizedUser } from "../components/AuthorizeView.js";
import DCandidates from "../components/DCandidates.js";
import { ToastProvider } from "react-toast-notifications";

const useStyles = makeStyles((theme) => ({
  root: {
    position: "absolute",
    top: theme.spacing(1), // Adjust as needed
    right: theme.spacing(6), // Adjust as needed
  },
}));

function Home() {
  const classes = useStyles();

  return (
    <ToastProvider>
      <AuthorizeView>
        <Tooltip title={<AuthorizedUser value="email" />} arrow>
          <span className={classes.root}>
            <Button variant="outlined" color="primary">
              <LogoutLink>
                Iesi acas'
              </LogoutLink>
            </Button>
          </span>
        </Tooltip>
        <DCandidates />
      </AuthorizeView>
    </ToastProvider>
  );
}

export default Home;
