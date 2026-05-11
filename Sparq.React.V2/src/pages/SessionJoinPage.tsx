import { GreenButton } from "@/components/buttons/greenButton";
import { LoadingIndicator } from "@/components/loadings/LoadingIndicator";
import {
  useJoinSessionMutation,
  useGetSessionPublicDataByIdQuery,
} from "@/features/session/sessionApi";
import { useGetCurrentUserQuery } from "@/features/user/userApi";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { pinCodeSchema } from "@/schemas/snapshot/pinCodeSchema";
import { nameSchema } from "@/schemas/user/nickNameSchema";
import type { JoinSessionRequestDto } from "@/features/session/sessionTypes";

export function SessionJoinPage() {
  const navigate = useNavigate();
  const { sessionId } = useParams();

  const { data: user, isLoading, error } = useGetCurrentUserQuery();
  const [joinFailed, setJoinFailed] = useState(false);

  const {
    data: sessionData,
    isLoading: isSessionLoading,
    error: sessionError,
  } = useGetSessionPublicDataByIdQuery(sessionId!, {
    skip: !sessionId,
  });

  const [joinSession, { isLoading: isJoining }] = useJoinSessionMutation();

  useEffect(() => {
    if (!isSessionLoading && !sessionData) {
      navigate("/session");
    }
  }, [isSessionLoading, sessionData, navigate]);

  const [nickname, setNickname] = useState("");
  const [pincode, setPincode] = useState("");
  const [errors, setErrors] = useState<{
    nickname?: string;
    pincode?: string;
  }>({});

  async function handleJoin() {
    setJoinFailed(false);
    const isGuest = !user;

    const resultPin = pinCodeSchema.safeParse(pincode);
    const resultNickname = isGuest
      ? nameSchema.safeParse(nickname)
      : { success: true as const };

    const newErrors: typeof errors = {};

    if (!resultPin.success) {
      newErrors.pincode = resultPin.error.issues[0]?.message ?? "Invalid PIN";
    }

    if (!resultNickname.success) {
      newErrors.nickname =
        resultNickname.error.issues[0]?.message ?? "Invalid nickname";
    }

    setErrors(newErrors);

    if (Object.keys(newErrors).length > 0) return;
    if (!sessionId) return;

    const payload: JoinSessionRequestDto = {
      sessionId,
      pinCode: pincode,
      nickname: isGuest ? nickname : user!.nickName,
    };

    try {
      const res = await joinSession(payload).unwrap();

      if (res.externalUserId) {
        localStorage.setItem(sessionId, res.externalUserId);
      }

      navigate(`/session/${sessionId}`);
    } catch (err) {
      setJoinFailed(true);
      console.error("Join failed:", err);
    }
  }

  if (isLoading || isSessionLoading) {
    return <LoadingIndicator />;
  }

  if (error || sessionError) {
    return <p>Something went wrong.</p>;
  }

  return (
    <div className="min-h-screen justify-center p-4">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <h1 className="text-xl">Join Session</h1>

        <GreenButton className="w-30 h-10" onClick={() => navigate(-1)}>
          Back
        </GreenButton>
      </div>

      <div className="w-full bg-[var(--surface-4)] p-5 rounded-lg gap-3 flex flex-col">
        <div>
          <p>Session: {sessionId}</p>
        </div>

        <div>
          <p>Displayname: {user?.nickName}</p>

          {!user && (
            <input
              type="text"
              placeholder="Enter your nickname"
              className="w-full p-2 border border-gray-300 rounded mt-2"
              value={nickname}
              onChange={(e) => setNickname(e.target.value)}
            />
          )}

          {errors.nickname && (
            <p className="text-[var(--error-text)]">{errors.nickname}</p>
          )}
        </div>

        <div>
          <p>PIN Code:</p>

          <input
            type="text"
            placeholder="Enter session PIN code"
            className={`w-full p-2 border rounded mt-2 transition-colors ${
              errors.pincode || joinFailed
                ? "border-red-500 focus:border-red-500 outline-none"
                : "border-gray-300"
            }`}
            value={pincode}
            onChange={(e) => setPincode(e.target.value)}
          />

          {errors.pincode && (
            <p className="text-[var(--error-text)]">{errors.pincode}</p>
          )}
        </div>

        <div className="flex justify-center">
          <GreenButton
            className="w-30 h-10 mt-4"
            onClick={handleJoin}
            disabled={isJoining}
          >
            Join
          </GreenButton>
        </div>
      </div>
    </div>
  );
}
