import { useEffect, useMemo } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useMemeEditor } from "../hooks/useMemeEditor";
import EditorControls from "../components/ControlsPanel";
import MemePreview from "../components/PreviewPane";

import { generateMeme } from "../api/memeApi";
import { downloadBlob } from "../utils/download";

type LocationState = { file?: File };

export default function MemeEditorPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const file = (location.state as LocationState | null)?.file;

  const {
    config,
    updateConfig,
    setImageFile,
    previewUrl,
    loadingPreview,
    configId,
    imageFile,
  } = useMemeEditor();

  const localUrl = useMemo(() => {
    if (!file) return null;
    return URL.createObjectURL(file);
  }, [file]);

  useEffect(() => {
    if (!file) {
      navigate("/");
      return;
    }
    setImageFile(file);

    return () => {
      if (localUrl) URL.revokeObjectURL(localUrl);
    };
  }, [file, navigate, setImageFile, localUrl]);

  async function onGenerate() {
    if (!imageFile || !configId) return;

    try {
      const blob = await generateMeme({ image: imageFile, configId });
      downloadBlob(blob, "meme.png");
    } catch (e) {
      console.error("Generate failed", e);
      alert("Generate failed. Check backend logs.");
    }
  }

  return (
    <div className="container py-4">
      <h1 className="display-6 fw-bold mb-4">Style Your Meme</h1>

      <div className="row g-4">
        <div className="col-12 col-lg-4">
          <EditorControls config={config} updateConfig={updateConfig} />
          <button
            className="btn btn-outline-secondary w-100"
            onClick={() => navigate("/")}
          >
            Back
          </button>
        </div>

        <div className="col-12 col-lg-8">
          <MemePreview imageUrl={previewUrl ?? localUrl} />

          <button
            className="btn btn-dark w-100 mt-3"
            onClick={onGenerate}
            disabled={!imageFile || !configId || loadingPreview}
          >
            Download 
          </button>
        </div>
      </div>
    </div>
  );
}
