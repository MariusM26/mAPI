import React, { useState, useEffect, createContext } from "react";
import { Navigate } from "react-router-dom";

const UserContext = createContext({});

function AuthorizeView(props) {
  const [authorized, setAuthorized] = useState(false);
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState({ email: "" });

  useEffect(() => {
    let retryCount = 0;
    const maxRetries = 10;
    const delay = 1000;

    function wait(delay) {
      return new Promise((resolve) => setTimeout(resolve, delay));
    }

    async function fetchWithRetry(url, options) {
      try {
        let response = await fetch(url, options);

        if (response.status === 200) {
          console.log("Authorized");
          let j = await response.json();
          setUser({ email: j.email });
          setAuthorized(true);
          return response;
        } else if (response.status === 401) {
          console.log("Unauthorized");
          console.log(response);
          return response;
        } else {
          throw new Error("" + response.status);
        }
      } catch (error) {
        retryCount++;
        if (retryCount > maxRetries) {
          throw error;
        } else {
          await wait(delay);
          return fetchWithRetry(url, options);
        }
      }
    }

      fetchWithRetry(`${process.env.REACT_APP_API_URL}/pingauth`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${localStorage.getItem("token")}`,
      },
    })
      .catch((error) => {
        console.log(error.message);
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  if (loading) {
    return (
      <>
        <p>Loading...</p>
      </>
    );
  } else {
    if (authorized && !loading) {
      return (
        <>
          <UserContext.Provider value={user}>
            {props.children}
          </UserContext.Provider>
        </>
      );
    } else {
      return (
        <>
          <Navigate to="/login" />
        </>
      );
    }
  }
}

export function AuthorizedUser(props) {
  const user = React.useContext(UserContext);

  if (props.value === "email") return <>{user.email}</>;
  else return <></>;
}

export default AuthorizeView;
