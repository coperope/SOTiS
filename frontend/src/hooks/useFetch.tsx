import React from "react";

const useFetch = (url: any, options: any) => {
  const [response, setResponse] = React.useState(null);
  const [error, setError] = React.useState(null);

  const fetchData = async () => {
    try {
      const res = await fetch(url, options);
      const json = await res.json();
      setResponse(json);
    } catch (error) {
      setError(error);
    }

    return {response, error}
  };

  return fetchData;
};

export default useFetch;