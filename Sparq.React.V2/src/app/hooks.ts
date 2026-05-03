import {
  type TypedUseSelectorHook,
  useDispatch,
  useSelector,
} from "react-redux";
import type { RootState, AppDispatch } from "./store";

// typed dispatch (thunkokhoz is)
export const useAppDispatch = () => useDispatch<AppDispatch>();

// typed selector (state shape is ismert lesz)
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
