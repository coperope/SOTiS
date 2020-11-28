import React, { useEffect, useState } from 'react';
import { getToken } from '../utils/authUtils';

const generateOptions =(
  method: string,
  body: object,
  token: string | null
) => {
  let options = {};
  if (method === "get"){
    options = {
      headers: {
        Authorization: `Bearer ${token}`,
        Accept: "application/json",
      },
      method: method,
    }
  } else {
    options = {
      headers: {
        Authorization: `Bearer ${token}`,
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      method: method,
      body: JSON.stringify(body),
    }
  }
  
  return options;
}

const useFetch = (
  url?: string,
  method: string = "get",
  body?: any,
  initialParams: object = {}) => {

  const [data, setData] = useState<any>(null);
  const [params, setParams] = useState(initialParams);
  const [hasError, setHasError] = useState(false);
  const [errorMessage, setErrorMessage] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const executeFetch = async (
    url: string,
    method: string = "get",
    body?: any
  ) => {

    setIsLoading(true);
    try {
      const token = getToken();
      console.log(token);
      const options = generateOptions(method, body, token);

      const response = await fetch(url, options);
      const result = await response.json();

      if (response.ok) {
        setData(result);
      } else {
        setHasError(true);
        setErrorMessage(result);
      }
    } catch (error) {
      setHasError(true);
      setErrorMessage(error.message);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(()=>{
    if(url) {
      executeFetch(url, method, body);
    }
  }, [url]);

  return {
    data,
    isLoading,
    hasError,
    errorMessage,
    executeFetch,
  };
};

export default useFetch;