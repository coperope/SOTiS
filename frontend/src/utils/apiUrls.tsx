export const BASE_URL = "http://localhost:5000";

export const LOGIN = `/users/authenticate`

export const GET_ALL_TESTS = `/student/tests`
export const GET_SINGLE_TEST_STUDENT = (student_id: string, test_id: string) => `/student/${student_id}/test/${test_id}`
