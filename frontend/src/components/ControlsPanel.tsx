import type {
  MemeConfig,
  TextAlign,
  WatermarkPosition,
} from "../models/MemeConfig";

type Props = {
  config: MemeConfig;
  updateConfig: <K extends keyof MemeConfig>(
    key: K,
    value: MemeConfig[K]
  ) => void;
};

const fonts = ["Impact", "Arial Black", "Arial", "Verdana", "Times New Roman"];
const watermarkPositions: WatermarkPosition[] = [
  "top-left",
  "top-right",
  "bottom-left",
  "bottom-right",
];

function clamp(n: number, min: number, max: number) {
  return Math.max(min, Math.min(max, n));
}

function fileToBase64(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = () => resolve(String(reader.result));
    reader.onerror = reject;
    reader.readAsDataURL(file);
  });
}

export default function ControlsPanel({ config, updateConfig }: Props) {
  async function onPickWatermark(e: React.ChangeEvent<HTMLInputElement>) {
    const f = e.target.files?.[0];
    if (!f) return;
    const b64 = await fileToBase64(f);
    updateConfig("watermarkImageBase64", b64);
  }

  return (
    <>
      <div className="card mb-3">
        <div className="card-body">
          <h6 className="fw-bold mb-3">Text</h6>

          <label className="form-label">Top text</label>
          <input
            className="form-control mb-3"
            value={config.topText}
            onChange={(e) => updateConfig("topText", e.target.value)}
          />

          <label className="form-label">Bottom text</label>
          <input
            className="form-control mb-3"
            value={config.bottomText}
            onChange={(e) => updateConfig("bottomText", e.target.value)}
          />

          <div className="form-check">
            <input
              id="allCaps"
              className="form-check-input"
              type="checkbox"
              checked={config.allCaps}
              onChange={(e) => updateConfig("allCaps", e.target.checked)}
            />
            <label className="form-check-label" htmlFor="allCaps">
              ALL CAPS
            </label>
          </div>
        </div>
      </div>

      <div className="card mb-3">
        <div className="card-body">
          <h6 className="fw-bold mb-3">Style</h6>

          <label className="form-label">Font</label>
          <select
            className="form-select mb-3"
            value={config.fontFamily}
            onChange={(e) => updateConfig("fontFamily", e.target.value)}
          >
            {fonts.map((f) => (
              <option key={f} value={f}>
                {f}
              </option>
            ))}
          </select>

          <div className="row g-2 mb-3">
            <div className="col-6">
              <label className="form-label">Font size (px)</label>
              <input
                type="number"
                className="form-control"
                value={config.fontSize}
                min={10}
                max={120}
                onChange={(e) =>
                  updateConfig(
                    "fontSize",
                    clamp(Number(e.target.value || 0), 10, 120)
                  )
                }
              />
            </div>

            <div className="col-6">
              <label className="form-label">Align</label>
              <select
                className="form-select"
                value={config.textAlign}
                onChange={(e) =>
                  updateConfig("textAlign", e.target.value as TextAlign)
                }
              >
                <option value="left">left</option>
                <option value="center">center</option>
                <option value="right">right</option>
              </select>
            </div>
          </div>

          <div className="row g-2 mb-3">
            <div className="col-6">
              <label className="form-label">Text color</label>
              <input
                type="color"
                className="form-control form-control-color w-100"
                value={config.textColor}
                onChange={(e) => updateConfig("textColor", e.target.value)}
              />
            </div>

            <div className="col-6">
              <label className="form-label">Stroke color</label>
              <input
                type="color"
                className="form-control form-control-color w-100"
                value={config.strokeColor}
                onChange={(e) => updateConfig("strokeColor", e.target.value)}
              />
            </div>
          </div>

          <label className="form-label d-flex justify-content-between">
            <span>Stroke width</span>
            <span className="text-muted">{config.strokeWidth}px</span>
          </label>
          <input
            type="range"
            className="form-range mb-3"
            min={0}
            max={10}
            step={1}
            value={config.strokeWidth}
            onChange={(e) =>
              updateConfig("strokeWidth", Number(e.target.value))
            }
          />

          <label className="form-label d-flex justify-content-between">
            <span>Padding</span>
            <span className="text-muted">{config.padding}px</span>
          </label>
          <input
            type="range"
            className="form-range"
            min={0}
            max={100}
            step={1}
            value={config.padding}
            onChange={(e) => updateConfig("padding", Number(e.target.value))}
          />
        </div>
      </div>

      <div className="card mb-3">
        <div className="card-body">
          <h6 className="fw-bold mb-3">Watermark (optional)</h6>

          <input
            type="file"
            accept="image/png,image/jpeg"
            className="form-control mb-3"
            onChange={onPickWatermark}
          />

          <label className="form-label">Position</label>
          <select
            className="form-select"
            value={config.watermarkPosition ?? "bottom-right"}
            onChange={(e) =>
              updateConfig(
                "watermarkPosition",
                e.target.value as WatermarkPosition
              )
            }
          >
            {watermarkPositions.map((p) => (
              <option key={p} value={p}>
                {p}
              </option>
            ))}
          </select>

          {config.watermarkImageBase64 ? (
            <button
              className="btn btn-sm btn-outline-danger mt-3"
              onClick={() => updateConfig("watermarkImageBase64", null)}
            >
              Remove watermark
            </button>
          ) : null}
        </div>
      </div>
    </>
  );
}
