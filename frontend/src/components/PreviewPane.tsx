type Props = {
  imageUrl: string | null;
};

export default function MemePreview({ imageUrl }: Props) {
  return (
    <div className="card">
      <div className="card-body">
        <div className="preview-box">
          {imageUrl ? (
            <img className="preview-img" src={imageUrl} alt="Preview" />
          ) : (
            <div className="text-muted">No image</div>
          )}
        </div>
      </div>
    </div>
  );
}
