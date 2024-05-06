import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button, makeStyles } from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  root: {
    display: "flex",
    flexDirection: "column",
    alignItems: "flex-start",
    position: "relative",
    padding: theme.spacing(2), // Add padding as needed
    borderRadius: theme.shape.borderRadius, // Apply border radius
    boxShadow: theme.shadows[3], // Apply shadow for depth
    "& form": {
      width: "100%",
    },
    "& button": {
      marginTop: theme.spacing(2), // Adjust margin top as needed
      marginRight: theme.spacing(7), // Add margin-right for spacing
      textTransform: "none", // Ensure no uppercase transformation
    },
    "& .error": {
      marginTop: theme.spacing(2),
      color: "red",
    },
  },
  buttonsContainer: {
    display: "flex",
    justifyContent: "space-between",
    width: "100%",
    marginTop: theme.spacing(2),
  },
}));


function Login() {
  // state variables for email and passwords
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  // state variable for error messages
  const [error, setError] = useState("");
  const navigate = useNavigate();

  // handle change events for input fields
  const handleChange = (e) => {
    const { name, value } = e.target;
    if (name === "email") setEmail(value);
    if (name === "password") setPassword(value);
  };

  const handleRegisterClick = () => {
    navigate("/register");
  };

  // handle submit event for the form
  const handleSubmit = (e) => {
    e.preventDefault();
    if (!email || !password) {
      setError("All fields must be filled");
    } else {
      setError("");
      var loginurl = "http://localhost:5187/login";

      fetch(loginurl, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: email,
          password: password,
        }),
      })
        .then(async (data) => {
          if (data.ok) {
            // setError("Successful Login.");

            const obj = await data.json();
            localStorage.setItem("token", obj.accessToken); // Store the token in localStorage
            window.location.href = "/";
          } else setError("Incorrect credentials");
        })
        .catch((error) => {
          console.error(error);
          setError("Error logging in..");
        });
    }
  };

  const classes = useStyles();

  return (
    <div className="containerbox">
      <h3>Login</h3>
      <form onSubmit={handleSubmit}>
        <div>
          <label className="forminput" htmlFor="email">
            Email:
          </label>
        </div>
        <div>
          <input
            type="email"
            id="email"
            name="email"
            value={email}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="password">Password:</label>
        </div>
        <div>
          <input
            type="password"
            id="password"
            name="password"
            value={password}
            onChange={handleChange}
          />
        </div>
        <div className={classes.buttonsContainer}>
          <Button
            type="submit"
            className={classes.button}
            variant="outlined"
            style={{ marginRight: 7 }}
          >
            Login
          </Button>
          <Button
            onClick={handleRegisterClick}
            className={classes.button}
            variant="outlined"
          >
            Register
          </Button>
        </div>
      </form>
      {error && (
        <p className="error" style={{ color: "red", fontStyle: "oblique" }}>
          {error}
        </p>
      )}
    </div>
  );
}

export default Login;
