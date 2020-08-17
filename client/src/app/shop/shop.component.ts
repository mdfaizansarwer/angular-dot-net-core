import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { ShopService } from './shop.service';
import { IPagination } from '../shared/models/pagination';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search', { static: false }) search: ElementRef;
  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  shopParams = new ShopParams();
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' }
  ]
  totalCount: number;
  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe(
      (response: IPagination) => {
        this.products = response.data;
        this.shopParams.pageSize = response.size;
        this.shopParams.pageNumber = response.index;
        this.totalCount = response.count;
      }
    );
  }

  getBrands() {
    this.shopService.getBrands().subscribe(
      (response: IBrand[]) => {
        this.brands = [{ id: 0, name: 'All' }, ...response];
      }
    );
  }

  getTypes() {
    this.shopService.getTypes().subscribe(
      (response: IType[]) => {
        this.types = [{ id: 0, name: 'All' }, ...response];
      }
    );
  }
  onBrandSelected(id) {
    this.shopParams.brandId = id;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onTypeSelected(id) {
    this.shopParams.typeId = id;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sort) {
    this.shopParams.sort = sort;
    this.getProducts();
  }
  onPageChanged(event: any) {
    if (this.shopParams.pageNumber == event) return;
    this.shopParams.pageNumber = event;
    this.getProducts();
  }
  onSearch() {
    this.shopParams.search = this.search.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onReset() {
    this.search.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }
}
