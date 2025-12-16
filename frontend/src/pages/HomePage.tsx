import { useNavigate } from "react-router-dom";
import { useRef } from "react";

export default function HomePage() {
  const navigate = useNavigate();
  const fileRef = useRef<HTMLInputElement>(null);

  function onPick(e: React.ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0];
    if (!file) return;

    navigate("/editor", { state: { file } });
    e.currentTarget.value = "";
  }

  return (
    <div className="container py-5">
      <div className="row justify-content-center">
        <div className="col-12 col-md-8 col-lg-6 text-center">
          <h1 className="display-5 fw-bold mb-3">Meme Generator</h1>

          <p className="text-muted mb-4">
            Upload a <strong>PNG</strong> or <strong>JPG</strong> image and
            customize it with text, fonts, colors, stroke, and watermark. You’ll
            see a live preview before downloading the final image.
          </p>

          <input
            ref={fileRef}
            type="file"
            accept="image/png,image/jpeg"
            hidden
            onChange={onPick}
          />

          <button
            className="btn btn-primary btn-lg px-5"
            onClick={() => fileRef.current?.click()}
          >
            Upload Image
          </button>

          <div className="form-text mt-3">
            Supported formats: PNG, JPG • Max quality preview
          </div>
        </div>
      </div>
    </div>
  );
}
