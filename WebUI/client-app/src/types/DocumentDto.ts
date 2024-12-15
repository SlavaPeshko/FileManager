import { DocumentType } from '../types/DocumentType'

export interface DocumentDto {
  id: number;
  name: string;
  type: DocumentType;
  previewPath: string;
  uploadAt: string;
  downloadCount: number;
}