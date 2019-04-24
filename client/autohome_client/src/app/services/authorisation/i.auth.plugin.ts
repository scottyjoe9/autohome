import { ISimpleEvent } from "strongly-typed-events";
import { User } from './user';

export interface IAuthPlugin{
    onUserUpdated:ISimpleEvent<User>;

    logout();
}