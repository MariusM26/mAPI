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
    "& .MuiButton-root": {
      textTransform: "none", // Ensure no uppercase transformation
    },
    "& .error": {
      marginTop: theme.spacing(2),
      color: "red",
    },
    // Add a new style for successful registration message
    success: {
      marginTop: theme.spacing(2),
      color: "green",
    },
  },
  buttonsContainer: {
    display: "flex",
    justifyContent: "space-between",
    width: "100%",
    marginTop: theme.spacing(2),
  },
}));

function Register() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const navigate = useNavigate();
  const [error, setError] = useState("");
  const [registrationStatus, setRegistrationStatus] = useState("");

  const handleLoginClick = () => {
    navigate("/login");
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    if (name === "email") setEmail(value);
    if (name === "password") setPassword(value);
    if (name === "confirmPassword") setConfirmPassword(value);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!email || !password || !confirmPassword) {
      setError("Please fill in all fields.");
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      setError("Please enter a valid email address.");
    } else if (password !== confirmPassword) {
      setError("Passwords do not match.");
    } else {
      setError("");
        fetch(`${process.env.REACT_APP_API_URL}/register`, {
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
            setRegistrationStatus("Successfully registered");
          } else {
            const test = await data.json();
            const errorMessages = Object.values(test.errors).flatMap(
              (messages) => messages
            );
            setError(errorMessages.join("<br />"));
          }
        })
        .catch((error) => {
          console.error(error);
          setError("Error registering.");
        });
    }
  };

  const classes = useStyles();

  return (
    <div className={`containerbox ${classes.root}`}>
      <h3>Register</h3>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="email">Email:</label>
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
        <div>
          <label htmlFor="confirmPassword">Confirm Password:</label>
        </div>
        <div>
          <input
            type="password"
            id="confirmPassword"
            name="confirmPassword"
            value={confirmPassword}
            onChange={handleChange}
          />
        </div>
        <div className={classes.buttonsContainer}>
          <Button
            type="submit"
            variant="outlined"
            style={{ marginRight: 7 }}
            className={classes.button}
          >
            Register
          </Button>
          <Button
            onClick={handleLoginClick}
            variant="outlined"
            className={classes.button}
          >
            Back to login
          </Button>
        </div>
      </form>
      {/* Render the error or success message */}
      {error && (
        <p
          className="error"
          style={{ color: "red", fontStyle: "oblique" }}
          dangerouslySetInnerHTML={{ __html: error }}
        />
      )}
      {registrationStatus && (
        <p
          className={classes.success}
          style={{ color: "green", fontStyle: "oblique" }}
        >
          {registrationStatus}
        </p>
      )}
    </div>
  );
}

export default Register;
