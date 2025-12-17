import { useEffect, useRef, useState } from "react";
import type { MemeConfig } from "../models/MemeConfig";
import { createConfig, updateConfigById } from "../api/configApi";
import { previewMeme } from "../api/memeApi";

const defaultConfig: MemeConfig = {
  topText: "",
  bottomText: "",
  fontFamily: "DejaVu Sans",
  fontSize: 40,
  textColor: "#ffffff",
  strokeColor: "#000000",
  strokeWidth: 3,
  textAlign: "center",
  padding: 20,
  allCaps: true,
  scaleDown: 0.25,
};

export function useMemeEditor() {
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [config, setConfig] = useState<MemeConfig>(defaultConfig);
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);
  const [loadingPreview, setLoadingPreview] = useState(false);

  const [configId, setConfigId] = useState<number | null>(null);
  const abortRef = useRef<AbortController | null>(null);

  function updateConfig<K extends keyof MemeConfig>(
    key: K,
    value: MemeConfig[K]
  ) {
    setConfig((prev) => ({ ...prev, [key]: value }));
  }

  useEffect(() => {
    return () => {
      if (previewUrl) URL.revokeObjectURL(previewUrl);
      abortRef.current?.abort();
    };
  }, [previewUrl]);

  useEffect(() => {
    if (!imageFile) return;

    const t = setTimeout(async () => {
      abortRef.current?.abort();
      const ac = new AbortController();
      abortRef.current = ac;

      try {
        setLoadingPreview(true);

        let id = configId;

        if (id == null) {
          id = await createConfig(config);
          setConfigId(id);
        } else {
          await updateConfigById(id, config);
        }

        const blob = await previewMeme(
          { image: imageFile, configId: id },
          ac.signal
        );

        if (previewUrl) URL.revokeObjectURL(previewUrl);
        setPreviewUrl(URL.createObjectURL(blob));
      } catch (e: any) {
        if (e?.name !== "AbortError") {
          console.error("Preview failed", e);
        }
      } finally {
        setLoadingPreview(false);
      }
    }, 400);

    return () => clearTimeout(t);
  }, [config, imageFile, configId]);

  return {
    imageFile,
    setImageFile,
    config,
    updateConfig,
    previewUrl,
    loadingPreview,
    configId,
  };
}
