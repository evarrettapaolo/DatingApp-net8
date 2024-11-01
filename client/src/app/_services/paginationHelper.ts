import { HttpParams, HttpResponse } from '@angular/common/http';
import { PaginatedResult } from '../_models/pagination';
import { signal } from '@angular/core';

// * intercept response and set passed signal parameter
export function setPaginatedResponse<T>(
  response: HttpResponse<T>,
  paginatedResultSignal: ReturnType<typeof signal<PaginatedResult<T> | null>>
) {
  paginatedResultSignal.set({
    items: response.body as T,
    pagination: JSON.parse(response.headers.get('Pagination')!),
  });
}

// * build pagination header, could add a predicate
export function setPaginationHeaders(pageNumber: number, pageSize: number) {
  let params = new HttpParams();
  if (pageNumber && pageSize) {
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
  }
  return params;
}
