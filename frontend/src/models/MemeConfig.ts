export type TextAlign = "left" | "center" | "right";
export type WatermarkPosition =
  | "top-left"
  | "top-right"
  | "bottom-left"
  | "bottom-right";

export interface MemeConfig {
  topText: string;
  bottomText: string;
  fontFamily: string;
  fontSize: number;
  textColor: string;
  strokeColor: string;
  strokeWidth: number;
  textAlign: TextAlign;
  padding: number;
  allCaps: boolean;
  scaleDown: number;

  watermarkImageBase64?: string | null;
  watermarkPosition?: WatermarkPosition;
}
