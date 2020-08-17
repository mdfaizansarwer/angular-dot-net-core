import { IProduct } from './product';

export interface IPagination {
    size: number;
    index: number;
    count: number;
    data: IProduct[];
}