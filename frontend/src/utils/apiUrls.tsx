export const BASE_URL = "http://localhost:5000";

export const LOGIN = `/users/authenticate`

export const REGISTER = `/users/register`

export const GET_ALL_TESTS = `/student/tests`

export const GET_ALL_KNOWLEDGE_SPACES = (professor_id: string) => `/professor/${professor_id}/knowledge_space`
export const GET_ONE_KNOWLEDGE_SPACE = (professor_id: string, knowledge_space_id: any) => `/professor/${professor_id}/knowledge_space/${knowledge_space_id}`

export const CREATE_TEST_PREFIX = `/professor/`
export const CREATE_TEST_POSTFIX = `/tests`

export const GET_SINGLE_TEST_STUDENT = (student_id: string, test_id: string) => `/student/${student_id}/test/${test_id}`
export const SUBMIT_TEST = (student_id: string, test_id: string) => `/student/${student_id}/test/${test_id}`

export const CREATE_KNOWLEDGE_SPACE = (professor_id: string) => `/professor/${professor_id}/knowledge_space`
export const GET_PROFESSOR_KNOWLEDGE_SPACES = (professor_id: string) => `/professor/${professor_id}/knowledge_space`
export const CREATE_REAL = (professor_id: string, knowledge_space_id: string) => `/professor/${professor_id}/knowledge_space/${knowledge_space_id}/real`
