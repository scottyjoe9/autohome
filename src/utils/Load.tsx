import * as Expo from 'expo'
import { IHashMap } from './IHashMap';

export class Load {

    private static layoutCache: IHashMap = {};

    public static async Layout(layoutName: string, require: any)
    {
        if (this.layoutCache[layoutName])
            return this.layoutCache[layoutName];

        let asset = Expo.Asset.fromModule(require);
        let response = await fetch(asset.uri);
        let layoutString = await response.text();

        this.layoutCache[layoutName] = layoutString;

        return layoutString;
    }
}