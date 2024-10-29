export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

export class PaginatedResult<T> {
  items?: T;  // * generic type but mainly used as members
  pagination?: Pagination;
}