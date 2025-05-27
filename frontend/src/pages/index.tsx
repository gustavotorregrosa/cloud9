import Image from "next/image";
import { SocketsContext } from "@/context/SocketsContext";
import { WebSocketService } from "@/services/websocketService";
import { Inter } from "next/font/google";
import { LoginOptions } from "@/components/LoginOptions";
import { useSelector, useDispatch } from "react-redux";
import { IState } from "@/store";
import { IUser } from "@/store/user/user.interface";
import { use, useContext, useEffect } from "react";
import { login } from "@/store/user/user.slice";

const inter = Inter({ subsets: ["latin"] });

export default function Home() {

  const dispatch = useDispatch();
  const user = useSelector<IState, IUser>(state => state.user)
  const websocketService = useContext(SocketsContext) as WebSocketService;

  const updateUserFromLocalStorage = () => {
      try {
        const userFromLocalStorage = JSON.parse(localStorage.getItem('user')!) as IUser
        if(user.access_token || !userFromLocalStorage.access_token) return
        dispatch(login(userFromLocalStorage))
      } catch (error) {
        console.log('Error parsing user from local storage')
      }
  }


  useEffect(() => {
    updateUserFromLocalStorage();
    websocketService.connect()
  }, [])

  return <>
    {!user.name && <LoginOptions />}
  </>
}
