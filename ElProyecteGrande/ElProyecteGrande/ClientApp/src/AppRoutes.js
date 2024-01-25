import  Counter  from "./components/Counter";
import  FetchUserData  from "./components/FetchUserData";
import  Home  from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-user-data',
    element: <FetchUserData />
  }
];

export default AppRoutes;
