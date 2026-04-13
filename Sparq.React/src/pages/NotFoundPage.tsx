import styles from "./NotFoundPage.module.css";
import { Link } from "react-router-dom";

export function NotFoundPage() {
  return (
    <div className={styles.notfound}>
      <h1 className={styles.code}>404</h1>

      <h2>Oops! Page not found</h2>

      <p>The page you are looking for does not exist.</p>

      <Link to="/" className={styles.link}>
        ← Back to Home
      </Link>
    </div>
  );
}
