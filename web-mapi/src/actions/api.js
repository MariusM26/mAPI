import axios from "axios";

const baseUrl = `${process.env.REACT_APP_API_URL}/api/`;

const api = {
  dCandidate(url = baseUrl + "dcandidate") {
    return {
      fetchAll: () => axios.get(url, {
        headers: {
          "Authorization": `Bearer ${localStorage.getItem("token")}`,
        }
      }),
      fetchById: (id) => axios.get(url + "/" + id, {
        headers: {
          "Authorization": `Bearer ${localStorage.getItem("token")}`,
        }
      }),
      create: (newRecord) => axios.post(url, newRecord, {
        headers: {
          "Authorization": `Bearer ${localStorage.getItem("token")}`,
        }
      }),
      update: (id, updateRecord) => axios.put(url + "/" + id, updateRecord, {
        headers: {
          "Authorization": `Bearer ${localStorage.getItem("token")}`,
        }
      }),
      delete: (id) => axios.delete(url + "/" + id, {
        headers: {
          "Authorization": `Bearer ${localStorage.getItem("token")}`,
        }
      }),
    };
  },
};

export default api;
