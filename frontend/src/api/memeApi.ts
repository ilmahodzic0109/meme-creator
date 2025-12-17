const BASE = import.meta.env.VITE_API_BASE_URL;

type PreviewArgs = {
  image: File;
  configId: number;
};

function buildFormData(args: PreviewArgs) {
  const fd = new FormData();
  fd.append("image", args.image);
  fd.append("configId", String(args.configId));
  return fd;
}

export async function previewMeme(
  args: PreviewArgs,
  signal?: AbortSignal
): Promise<Blob> {
  const res = await fetch(`${BASE}/api/meme/preview`, {
    method: "POST",
    body: buildFormData(args),
    signal,
  });

  if (!res.ok)
    throw new Error(await res.text().catch(() => `HTTP ${res.status}`));
  return await res.blob();
}

export async function generateMeme(args: PreviewArgs): Promise<Blob> {
  const res = await fetch(`${BASE}/api/meme/generate`, {
    method: "POST",
    body: buildFormData(args),
  });

  if (!res.ok)
    throw new Error(await res.text().catch(() => `HTTP ${res.status}`));
  return await res.blob();
}
