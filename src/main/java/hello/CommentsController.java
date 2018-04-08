package hello;

import hello.dbCalls.Add;
import hello.dbCalls.Comments;
import hello.dbCalls.UserPage;
import org.json.JSONArray;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.sql.ResultSet;
import java.sql.SQLException;

@RestController
public class CommentsController {
    @RequestMapping(value = "/addComment")
    public void addComment(@RequestParam(value = "id", required = true) int id,
                     @RequestParam(value = "questionId", required = true) int questionId,
                     @RequestParam(value = "comment", required = true) String comment,
                     @RequestParam(value = "previousCommentId", required = false, defaultValue = "0") int previousCommentId) {
        MsSqlConnection connection = new MsSqlConnection();

        Comments.addComment(connection.con, id, questionId, comment, previousCommentId);
    }

    @RequestMapping(value = "/addCommentFeedback")
    public void addCommentFeedback(@RequestParam(value = "id", required = true) int id,
                           @RequestParam(value = "commentId", required = true) int commentId,
                           @RequestParam(value = "feedback", required = true) int feedback) {
        MsSqlConnection connection = new MsSqlConnection();

        Comments.addCommentFeedback(connection.con, id, commentId, feedback);
    }

    @RequestMapping(value = "/getComments")
    public String getComments(@RequestParam(value = "id", required = true) int id,
                                   @RequestParam(value = "questionId", required = true) int questionId,
                                   @RequestParam(value = "amount", required = false, defaultValue = "25") int amount) {
        MsSqlConnection connection = new MsSqlConnection();

        ResultSet userInfo = Comments.getComments(connection.con, id, questionId, amount);

        JSONArray arr = new JSONArray();
        try {
            try {
                arr = ResultSetConverter.convert(userInfo);
            } catch (org.json.JSONException e) {
                e.printStackTrace();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        return arr.toString();
    }
}
