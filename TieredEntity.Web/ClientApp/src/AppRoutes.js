import { Demo } from "./components/Demo";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/demo',
    element: <Demo />
  }
];

export default AppRoutes;
