import { useNavigate } from "react-router-dom";

function LogoutLink(props) {
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();
    fetch("http://localhost:5187/logout", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: "",
    })
      .then((data) => {
        if (data.ok) {
          localStorage.removeItem("token");
          navigate("/login");
        }
      })
      .catch((error) => {
        console.error(error);
      });
  };

  return (
    <>
      <a href="#" onClick={handleSubmit}>
        {props.children}
      </a>
    </>
  );
}

export default LogoutLink;
